# 3D Runner - Тестовое задание

---
## Управление
- Touch & swipes 

## Описание
ТЗ - находится в папке Docs

Мой подход: Unity КОП + DI + Работа с редактором больше чем с кодом.

В проекте используются DI библиотека [VContainer](https://github.com/hadashiA/VContainer) 
,также [UniTask](https://github.com/Cysharp/UniTask)

Основной упор по ТЗ на расширяемость/настройку собираемых эффектов на уровне.

При проэктировании были использованы принципы:
1. SRP,DIP,OCP,ISP
2. В качестве паттернов:
   - Фабрика и пул объектов
   - GameLoop
   - Простая реализация FSM на enum
   - Команда BaseCollectableEffectAction

Использовалась моя редакторская тулза для работы со сценами в проекте [Scenes Window](https://gitlab.com/p1284/scenes_window)
