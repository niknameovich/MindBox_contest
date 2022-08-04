using System.Collections.Generic;
using System.Reflection;

namespace Geometry_Teacher
{
// Абстрактный базовый класс реализущий единое логирование ошибок
public abstract class Logger
    {
        protected const string side_error_message = "Сторона треугольника должна быть положительным числом";
        public static string logFilePath = @"C:\Logs\Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
        private static void WriteLog(string strLog)
{
    FileInfo logFileInfo = new FileInfo(logFilePath);
    DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName??"");
    if (!logDirInfo.Exists) logDirInfo.Create();
    using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
    {
        using (StreamWriter log = new StreamWriter(fileStream))
        {
            log.WriteLine(strLog);
        }
    }
}
public static void prepareErrorMessage_output(string message)
{
    WriteLog(message);
    throw new NotImplementedException($"ERROR! Look for advanced info here - {logFilePath}");
}
    }

// Интерфейс для уверенности в себе ... Апкаст производных классов для вызова метода получения площади в рантайме 
public interface IGeometryObject
{
    public double Calculate_Square();
}
//Конкретный класс реализующий поведение треугольника
internal class Triangle:Logger,IGeometryObject
{
    public Triangle(double a, double b, double c)
    {
        try
        {
            this.a_side = a;
            this.b_side = b;
            this.c_side = c;
        }
        catch (NotImplementedException ex)
        {
            prepareErrorMessage_output($"{ex.Message}\r\n{ex.StackTrace}");
        }
    }

private double a;
// Проверка на положительное число, вычисление полусуммы
// Не хватает логики проверки были ли внесены все три стороны, функционал сброса полусуммы в каком то бизнес случае. 
public double a_side {get {return this.a;} set{
    if (value<=0) prepareErrorMessage_output(side_error_message);
    else {this.a = value; 
    this.p+=value/2;}
}}
private double b; 
public double b_side{get {return this.b;} set{
    if (value<=0) prepareErrorMessage_output(side_error_message);
    else 
    {
        this.b = value;
        this.p+=value/2;
    }
}}
private double c;
public double c_side{get {return this.c;} set{
    if (value<=0) prepareErrorMessage_output(side_error_message);
    else 
    {
        this.c = value;
        this.p+=value/2;
    }
}}
private double p;
//Вычисление площади треугольника по трем сторонам, проверки на прямоугольность и корректность треугольника
public double Calculate_Square()
{
        var result =  Math.Sin(a/b) == 1 ? 0.5*a*b : 
                    Math.Sin(b/c) == 1 ? 0.5*b*c:
                    Math.Sin(a/c) == 1 ? 0.5*a*c:
                    Math.Sqrt(p*(p-a)*(p-b)*(p-c));
        return result != 0 ? result: double.NaN;
}
//Деприкейтед
public bool Detect90DegreeAngle()
{
    return Math.Sin(a/b) == 1 ? true : 
        Math.Sin(b/c) == 1 ? true:
            Math.Sin(a/c) == 1 ? true:
                false;
}
}
//Конкретный класс реализующий поведение окружности
internal class Cyrcle: Logger,IGeometryObject
{
    public Cyrcle(double radius)
    {
        this.radius = radius;
    }
private double r;
public double radius {
    get {return this.r;} 
    set
    {
    if (value<=0) prepareErrorMessage_output(side_error_message);
    else {this.r = value;}
    }
    }
public double Calculate_Square()
{
    double result = Math.PI*Math.Pow(r,2);
    return result != 0 ? result: double.NaN;
}
}
// Класс ученого исследователя, использующий рефлексию для определения сигнатуры класса, необходимого для рассчета
public class Scientist:Logger
{
    public Scientist(string geometryType, params object[] args)
    {
        Type current_geometry_class = null; 
            try{current_geometry_class=Type.GetType($"Geometry_Teacher.{geometryType}",true, true);}
            catch(TypeLoadException ex) {prepareErrorMessage_output($"{ex.Message}\r\n{ex.StackTrace}");}
        if (current_geometry_class != null)
            {
                if (args.Length > 0)
                {
            
                if (current_geometry_class.GetInterface("IGeometryObject")!=null)
                {
                    var geo_constructor = current_geometry_class.GetConstructors().First();
                    var geo_atrs = geo_constructor.GetParameters();
                    if (geo_atrs.Count() == args.Length)
                        this.geometry_setter = (IGeometryObject)geo_constructor.Invoke(args);
                    else
                        prepareErrorMessage_output($"There is no constructor with {args.Length} params in {geometryType}");
                }
                else
                {
                    prepareErrorMessage_output($"ADMIN ERROR ! Your {geometryType} is not realizes IGeometryObject Interface");
                }
                }
                else
                {
                    prepareErrorMessage_output($"ERROR! Your request to {geometryType} dont have edges and cannot be calculated");
                }
            }
            else
            {
                prepareErrorMessage_output($"ERROR ! There is no {geometryType} class defined for calculations.");
            }
                
    }
private IGeometryObject currentGeometry;
public IGeometryObject geometry_setter{
    set
     {if (value != null) currentGeometry = value;
     else prepareErrorMessage_output("Something went wrong");}
     }
// Вызов метода определенного в общем интерфейсе геометрических фигур для текущего геометрического инстанса обнаруженного исследователем 
public double askToCalculate()
{
    double result =  this.currentGeometry.Calculate_Square();
    if (result == double.NaN)
        prepareErrorMessage_output("Your geometry configuration is incorrect");
    return result;    
}
}

}

// Консольный клиент, вместо автотестов ... 
public static class Console_Client
{
    public static void Main(string[] args)
    {
        var teacher = new Geometry_Teacher.Scientist("Triangle",4,4,5);
        var result = teacher.askToCalculate();
        Console.WriteLine(result);
        Console.Read();
    }
}