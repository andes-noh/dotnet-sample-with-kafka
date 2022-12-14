# dotnet-basic-with-kafka

## .NET Template kafka sample Project - summary

local PC:

-   zookeeper, server 실행
    ![setting](./kaf.png)

bootstrapServer:

-   `localhost:9092` default

producer:

-   매초 `hello kafka world` 메시지 발행

consumer:

-   자동 토픽 생성 기능 추가 - 22.08.17
-   `hello kafka world` 메시지 출력

collector:

-   BackgroundService로 `hello world` 한번 출력

## Setup

> [.NET CLI 개요](https://docs.microsoft.com/ko-kr/dotnet/core/tools/)

```
$Env:DOTNET_CLI_UI_LANGUAGE = "en"

dotnet new sln
dotnet new console -o app
dotnet sln add app
dotnet new xunit -o test
dotnet sln add test
dotnet add test/test.csproj reference app/app.csproj
dotnet new nugetconfig
dotnet restore
```

> [kafka 환경 준비 및 실행]

-   하단 블로그 참조
-   window: https://herojoon-dev.tistory.com/118
-   mac: https://somjang.tistory.com/entry/Kafka-Mac에-카프카-설치하고-실행해보기

## 패키지 추가/제거

> .csproj 파일을 선택하고 오른쪽 클릭 -> `Visual Nuget: Manage Packages` 선택

```
dotnet add package dotenv.net
dotnet remove package Newtonsoft.Json
```

## Run

```
dotnet run --project app
```

## Run Tests

```
dotnet test
```

## EXE

```
dotnet publish \
  --output "./dist" \
  --configuration Release \
  --self-contained true
```

---

## References

-   [[Docs / Visual Studio / MSBuild / MSBuild reference] Common MSBuild project items](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items)
-   [Dependency injection in .NET](https://docs.microsoft.com/ko-kr/dotnet/core/extensions/dependency-injection)
-   [Create a Windows Service using BackgroundService](https://docs.microsoft.com/ko-kr/dotnet/core/extensions/windows-service)
-   [dotenv.net](https://github.com/bolorundurowb/dotenv.net)
