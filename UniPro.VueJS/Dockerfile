# Этап 1: Сборка приложения
FROM node:18-alpine AS build
WORKDIR /app
# Копируем файлы package.json и package-lock.json для установки зависимостей
COPY package.json package-lock.json ./
RUN npm install
# Копируем весь исходный код
COPY . .
# Запускаем сборку (на выходе обычно формируется папка dist)
RUN npm run build

# Этап 2: Раздача статики через Nginx
FROM nginx:alpine AS runtime
# Опционально: удалить стандартные файлы Nginx
RUN rm -rf /usr/share/nginx/html/*
# Копируем собранные файлы из этапа сборки в папку, откуда nginx раздаёт статику
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
