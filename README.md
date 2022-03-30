# SfC Appointment System Backend Api

A Backend api for MadeTech Academy 2022 Jan Team Project.
( Note: This project was created for study purpose. It is not related to any real-world service provided by Skills For Care)

Hosted demo: https://skillsforcare-api.herokuapp.com/

## To run on your local machine

#### With docker-compose:
```
docker-compose up -d
```
This should starts a webapi server and postgres db embedded in docker.

Visit http://localhost:5002 and you should see the base endpoint.

After you played with it, remember to shutdown the server by `docker-compose down`.

#### Without docker

You need to setup a local postgres db at your machine first.

After you have done that, open `project/appsettings.Development.json` and change the value of `DefaultConnection` to match the settings of your database.

Then, you need to setup the table in your database.
To do that, install [dotnet-ef tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet), then run the below command:

```shell
cd project
dotnet ef database update --context PsqlDbContext 
```

After that, run `dotnet run` to start up the local server.





[TODO]
(tidy up setup instruction and ref materials later)


## Reference

(used this way to push to heroku)
https://devcenter.heroku.com/articles/build-docker-images-heroku-yml


(about postgres db connection string on heroku:)
https://n1ghtmare.github.io/2020-09-28/deploying-a-dockerized-aspnet-core-app-using-a-postgresql-db-to-heroku/
https://github.com/jeremymaya/herokufy-dotnet
https://theanzy.github.io/blog/.net/core/2021/05/13/Deploy-AspNET-Core-MVC-to-docker-Heroku.html


(took some reference but didn't used this way eventually)
https://medium.com/@vnqmai.hcmue/deploy-asp-net-core-to-heroku-for-free-using-docker-bd6d6fc161ae
https://github.com/claresudbery/dotnet-docker-clare

## Note for myself

**V. IMPORTANT, REMEMBER TO SET THIS BEFORE 1st time PUSHING**
```shell
heroku stack:set container
```

#### to pass $PORT into docker locally:
```shell
cd project
docker build -t dotnet-dockerapp-prod .
docker run -d --rm -p 5002:5002 -e PORT=5002 dotnet-dockerapp-prod
```
