# KodyPromocyjneAPI
Prosta aplikacja do zarządzania kodami promocyjnymi - REST API.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)

## General info
Aplikacja pozwala na dodawanie, zmianę nazwy, pobranie oraz usunięcię kodu. Dodatkowo jest możliwość ustawienia kodu jako nieaktywny. Każdy kod można pobrać określoną ilość razy.

Dodając nowy kod należy podać nazwę (inną niż pozostałe aktywne i dostępne kody) oraz ilość możliwych pobrań. Kod kodu zostanie nadany automatycznie jako unikalny nr Guid. Gdy kod istnieje w bazie można zmienić jego nazwę oraz ustawić jako nieaktywny. Aplikacja posiada możliwość wyświetlenia wszystkich aktywnych i dostępnych(ilość możliwych pobrań >0) kodów lub jednego wybranego po Id.  

Historia zmian przechowywana jest w tabeli ChangeLogs. Aby pobrać historię zmiany dla konkretnego Kodu promocyjnego, należy wpisać jego Id (w części ChangeLog).

## Technologies
Project is created with:
* EntityFrameworkCore
* AspNetCore
* Unity

## Setup
Aby uruchomić projekt, zainstaluj go lokalnie i uruchom KodyPromocyjneAPI. Sprawdz czy aplikacja działa poprawnie: http://localhost:10500/api/status
lub skorzystaj ze Swaggera, gdzie otrzymasz dostęp do wszystkich Controllerów: http://localhost:10500/swagger/index.html