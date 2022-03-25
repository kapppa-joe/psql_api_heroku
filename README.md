# Demo .NET webapi with postgres on Heroku

A walking skeleton of webapi with .NET and postgres.
I started this project to test deploying .NET and postgres app in Heroku.

Hosted demo: https://sleepy-sands-79356.herokuapp.com/


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
cd project
docker build -t dotnet-dockerapp-prod .
docker run -d --rm -p 5002:5002 -e PORT=5002 dotnet-dockerapp-prod