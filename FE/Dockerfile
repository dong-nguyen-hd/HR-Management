#docker build -t hr-management-fe .
FROM node:12.22.12
WORKDIR /usr/src/app/fe

COPY package*.json ./
COPY quasar.conf.js ./

RUN npm install
RUN npm install -g @vue/cli
RUN npm install -g @quasar/cli 

COPY . .
RUN quasar build

CMD ["quasar", "dev"]