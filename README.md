# Вычисление площади геометрических фигур
* В качестве compile-time определителя выбран подход с рефлексией и отдельным классом исследователем (а-ля паттерн строитель)
* Добавлен абстрактный класс логгера, для единообразного вывода ошибок (логика основного метода нагуглена, внешние библиотеки для логирования не хотелось подключать)
* Автотесты увы не писал, есть такой пробел в знаниях

## В коде учтены следующие граничные случаи для тестов: 
* проверка значений длин на положительность
* проверка фигуры на корректность. Площадь больше нуля
* проверка на существование в модуле выбранного клиентом класса геометрии
* проверка что выбранный класс реализует интерфейс для подсчета площади фигуры
* проверка на соответствие типа геометрической фигуры и количества сторон, переданных клиентом
* проверка на ненулевую длину переданных аргументов (Дублирует предыдущую проверку, используется как верхнеуровневый флаг разрешения рассчетов)
