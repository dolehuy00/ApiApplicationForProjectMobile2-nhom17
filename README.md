# UserAPIApication

Dự án api cho ứng dụng xem phim trên android
### # Ứng dụng được xây dựng trên nền tảng asp.net core web api
- Để gọi dưới localhost thông qua ip của máy thì dự án cần được triển khai lên IIS
- Khi đã có IIS thì IIS cần đến donet hosting bundle để chạy ứng dụng asp.net core
- Download donet hosting bundle: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.3-windows-hosting-bundle-installer


## API Reference

### Request login
```
  POST /api/User/login
```
Body JSON object
| Attributes | Type     | Description                |
| :--------  | :------- | :------------------------- |
| `email`    | `string` | **Required**               |
| `password` | `string` | **Required**               |



### Request register
```
  POST /api/User/register
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `name`     | `string` | **Required** |
| `email`    | `string` | **Required** |
| `password` | `string` | **Required** |
| `passwordconfirm`     | `string` | **Required** |



### Request change password
```
  POST /api/User/change-password
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `oldPassword`     | `string` | **Required** |
| `email`    | `string` | **Required** |
| `newPassword` | `string` | **Required** |
| `passwordconfirm`     | `string` | **Required** |



### Request get all histories
```
  GET /api/History/all/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request get limit the newest histories
```
  GET /api/History/{limit}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `limit`     | `int` | **Required** |
| `userId`     | `int` | **Required** |



### Request get limit skip-take the newest histories
```
  GET /api/History/{skip}/{take}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `skip`     | `int` | **Required** |
| `take`     | `int` | **Required** |
| `userId`     | `int` | **Required** |

For example: With request `/api/History/all/2/20/1`.
In request:
  `skip  = 2`,
  `take = 20`,
  `userId = 1`.
The server will respond to the latest history from 3 to 22 (skip record 1,2 and take the next 20 records).



### Request add new or update history
```
  GET /api/History/add
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |
| `watchedDate`    | `DateTime` | **Required**. ISO 8601 format (dd/MM/yyyyThh:mm:ss). UTC datetime|
| `secondsCount` | `int` | **Required** |
| `informationMovie`     | `string` | **Required** |


### Request delete a history
```
  DELETE /api/History/delete-one/{historyId}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `historyId`     | `int` | **Required** |
| `userId`     | `int` | **Required** |




### Request delete many history
```
  DELETE /api/History/delete-many/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |

* Body is a JSON array `historyIds` type `integer`
+ For example: `[1,2,3]`



### Request delete last hour history
```
  DELETE /api/History/delete-last-hour/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete last day history
```
  DELETE /api/History/delete-last-day/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete last week history
```
  DELETE /api/History/delete-last-week/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete last month history
```
  DELETE /api/History/delete-last-month/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete all history
```
  DELETE /api/History/delete-all/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request get all playlist
```
  GET /api/WatchList/all/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request add a new playlist
```
  POST /api/WatchList/add
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |
| `title`    | `string` | **Required**|


### Request edit a new playlist
```
  POST /api/WatchList/edit/{watchListId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |

Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `id`     | `int` | **Required** |
| `userId`     | `int` | **Required** |
| `title`    | `string` | **Required**|




### Request delte a playlist
```
  DELETE /api/WatchList/delete-one/{watchListId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |




### Request delete many playlist
```
  DELETE /api/WatchList/delete-many
```
* Body is a JSON array `playListIds` type `integer`
+ For example: `[1,2]`


### Request get all playlist items
```
  GET /api/WatchListItem/all/{watchListId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |


### Request add a new playlist item
```
  POST /api/WatchListItem/add
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |
| `informationMovie`    | `string` | **Required**|


### Request check a movie is exist in playlist
```
  GET /api/WatchListItem/check
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |
| `informationMovie`    | `string` | **Required**|




### Request delte a playlist item
```
  DELETE /api/WatchListItem/delete-one/{watchListItemId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListItemId`     | `int` | **Required** |




### Request delete many playlist item
```
  DELETE /api/WatchListItem/delete-many
```
* Body is a JSON array `playListItemIds` type `integer`
+ For example: `[1,2]`



### Request get all review video
```
  GET /api/ReviewVideo/all/{informationMovie}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `informationMovie`     | `string` | **Required** |


### Request add a new review video
```
  POST /api/ReviewVideo/add
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `informationReviewVideo`     | `string` | **Required** |
| `informationMovie`    | `string` | **Required**|



### Request delte a review video
```
  DELETE /api/ReviewVideo/delete-one/{reviewVideoId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `reviewVideoId`     | `int` | **Required** |