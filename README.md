1. На хост ubuntu перенести в папку проекта папки Realese и sources (wine-mono-7.0.0-x86.msi)
   В папке Realese должны лежать dll-файлы из папки lib.
2. Перенести Dockerfile и docker-compose.yml
3. В терминале перейти в папку проекта и запустить сборку docker образа:
   docker-compose down && docker-compose up -d --no-deps --build
4. Чтобы остановить (удалить) образ:
   docker-compose down
5. Чтобы посмотреть логи:
   docker-compose logs
6. Чтобы запустить если не запускается образ:
   docker run --rm -p 5104:5104 -it phdapi bash

P.S.: https://dl.winehq.org/wine/wine-mono/7.0.0/

# Дополнительные команды:

docker image prune - удаление лишних образов
docker system df - сколько пространства занимает docker
docker container prune - удаление остановленых контейнеров

Чтобы подключиться к контейнеру с проектом:
docker exec -it electricitysckk_electricitysckk_1 bash

Чтобы смотреть логи контейнера  
docker-compose logs -f electricitysckk

Любые изменения в контейнерах НЕ сохранятся если остановить контейнер, для этого нужно любые изменения закоммитить в образ (гугл).

# Дополнительная инфа

10.1.15.244 является виртуальным роутром, который по запросу на порт 5105 перебрасывает все пакеты на 10.1.15.241:5105

В свою очередь 10.1.15.241 является ubuntu серевер, на котором установлен docker.
Docker слушает порт 5105 и отправляет запросы в контейнер с проектом.

# Доступ к хосту по SSH:

ssh demi@10.1.15.241
Пароль: Ace345@W
