FROM ubuntu:focal

WORKDIR /app
EXPOSE 5104

COPY /sources/ /sources/

#Переменные окружения для старта Wine
ENV WINEDEBUG=fixme-all
ENV WINEPREFIX=/root/.net

#Добавляем поддержку x86
RUN dpkg --add-architecture i386 \
    && apt-get update \
    # Ставим некоторые необходимые пакеты
    && apt-get install -qfy --install-recommends \
        software-properties-common \ 
        wget \
        gnupg2 \
    # Добавляем репозитарий Wine
    && wget -nc https://dl.winehq.org/wine-builds/winehq.key \
    && apt-key add winehq.key \
    && apt-add-repository 'deb https://dl.winehq.org/wine-builds/ubuntu/ focal main' \
    && apt-get install -qfy --install-recommends winehq-stable \
    && apt-get install -qfy winbind \
    && wine /sources/wine-mono-7.0.0-x86.msi \
    # Подчищаем лишнее
    && apt-get -y clean \
    && rm -rf \
      /var/lib/apt/lists/* \
      /usr/share/doc \
      /usr/share/doc-base \
      /usr/share/man \
      /usr/share/locale \
      /usr/share/zoneinfo \
      /sources

#Копируем наше приложение
COPY /Release/ .
