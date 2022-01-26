1. На хост ubuntu перенести в папку проекта папки Realese и sources (wine-mono-7.0.0-x86.msi)
2. Перенести Dockerfile и docker-compose.yml
3. В терминале перейти в папку проекта и запустить сборку docker образа:
   docker-compose up -d --no-deps --build
4. Чтобы остановить (удалить) образ:
   docker-compose down
5. Чтобы посмотреть логи:
   docker-compose logs
6. Чтобы запустить если не запускается образ:
   docker run --rm -p 5104:5104 -it phdapi bash

P.S.: https://dl.winehq.org/wine/wine-mono/7.0.0/
