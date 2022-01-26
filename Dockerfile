FROM ubuntu:focal

WORKDIR /app
EXPOSE 5104

COPY /sources/ /sources/

#Добавляем поддержку x86
RUN dpkg --add-architecture i386 \
    && apt-get update \
    #Ставим некоторые необходимые пакеты
    && apt-get install -qfy --install-recommends \
        software-properties-common \ 
        wget \
        gnupg2 \
    #Добавляем репозитарий Wine
    && wget -nc https://dl.winehq.org/wine-builds/winehq.key \
    && apt-key add winehq.key \
    && apt-add-repository 'deb https://dl.winehq.org/wine-builds/ubuntu/ focal main' \
    && apt-get install -qfy --install-recommends winehq-stable \
    && apt-get install -qfy winbind \
    && wine /sources/wine-mono-7.0.0-x86.msi \
    #Подчищаем лишнее
    && apt-get -y clean \
    && rm -rf \
      /var/lib/apt/lists/* \
      /usr/share/doc \
      /usr/share/doc-base \
      /usr/share/man \
      /usr/share/locale \
      /usr/share/zoneinfo \
      /sources
#Переменные окружения для старта Wine
ENV WINEDEBUG=fixme-all
ENV WINEPREFIX=/root/.net
ENV WINEARCH=win64
#Пуск конфигурирования Wine
# RUN winecfg \
#     #Скачиваем winetricks, без них .Net Framework не заведётся
#     && wget https://raw.githubusercontent.com/Winetricks/winetricks/master/src/winetricks \
#     -O /usr/local/bin/winetricks \
#     && chmod +x /usr/local/bin/winetricks \
#Подчищаем лишнее
    # && apt-get -y clean \
    # && rm -rf \
    #   /var/lib/apt/lists/* \
    #   /usr/share/doc \
    #   /usr/share/doc-base \
    #   /usr/share/man \
    #   /usr/share/locale \
    #   /usr/share/zoneinfo \
    # #Запуск Wine с необходимыми дополнениями
    # && wineboot -u && winetricks -q dotnet472

#Копируем наше приложение
COPY /Release/ .
# ENTRYPOINT ["wine", "phd_api.exe"]
