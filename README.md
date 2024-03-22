# UserAPIApication

Dự án api cho ứng dụng xem phim trên android
### # Ứng dụng được xây dựng trên nền tảng asp.net core web api
- Để gọi dưới localhost thông qua ip của máy thì dự án cần được triển khai lên IIS
- Khi đã có IIS thì IIS cần đến donet hosting bundle để chạy ứng dụng asp.net core
- Download donet hosting bundle: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.3-windows-hosting-bundle-installer


* Đối với các phương thức có [Authorize] cần gửi kèm trong header `Key: Authorization` và `Value: Bearer <token>`.

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



### Request change password [Authorize]
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



### Request forgot password
```
  POST /api/User/forgot-password-request
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `email`    | `string` | **Required** |


### Request forgot password check code
```
  POST /api/User/forgot-password-checkcode
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `code`     | `int` | **Required** |
| `email`    | `string` | **Required** |



### Request forgot password change password
```
  POST /api/User/forgot-password-change
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `code`     | `int` | **Required** |
| `email`    | `string` | **Required** |
| `password` | `string` | **Required** |
| `passwordConfirm`     | `string` | **Required** |



### Request get all histories [Authorize]
```
  GET /api/History/all/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request get limit the newest histories [Authorize]
```
  GET /api/History/{limit}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `limit`     | `int` | **Required** |
| `userId`     | `int` | **Required** |



### Request get limit skip-take the newest histories [Authorize]
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



### Request add new or update history [Authorize]
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


### Request delete a history [Authorize]
```
  DELETE /api/History/delete-one/{historyId}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `historyId`     | `int` | **Required** |
| `userId`     | `int` | **Required** |




### Request delete many history [Authorize]
```
  DELETE /api/History/delete-many/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |

* Body is a JSON array `historyIds` type `integer`
+ For example: `[1,2,3]`



### Request delete last hour history [Authorize]
```
  DELETE /api/History/delete-last-hour/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete last day history [Authorize]
```
  DELETE /api/History/delete-last-day/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete last week history [Authorize]
```
  DELETE /api/History/delete-last-week/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete last month history [Authorize]
```
  DELETE /api/History/delete-last-month/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request delete all history [Authorize]
```
  DELETE /api/History/delete-all/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request get all playlist [Authorize]
```
  GET /api/WatchList/all/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |



### Request add a new playlist [Authorize]
```
  POST /api/WatchList/add
```
Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |
| `title`    | `string` | **Required**|


### Request edit a new playlist [Authorize]
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




### Request delte a playlist [Authorize]
```
  DELETE /api/WatchList/delete-one/{watchListId}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |
| `userId`     | `int` | **Required** |




### Request delete many playlist [Authorize]
```
  DELETE /api/WatchList/delete-many/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |

* Body is a JSON array `playListIds` type `integer`
+ For example: `[1,2]`


### Request get all playlist items [Authorize]
```
  GET /api/WatchListItem/all/{watchListId}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |
| `userId`     | `int` | **Required** |


### Request add a new playlist item [Authorize]
```
  POST /api/WatchListItem/add/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |

Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |
| `informationMovie`    | `string` | **Required**|


### Request check a movie is exist in playlist [Authorize]
```
  GET /api/WatchListItem/check/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |

Body JSON object
| Attributes | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListId`     | `int` | **Required** |
| `informationMovie`    | `string` | **Required**|




### Request delete a playlist item [Authorize]
```
  DELETE /api/WatchListItem/delete-one/{watchListItemId}/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `watchListItemId`     | `int` | **Required** |
| `userId`     | `int` | **Required** |




### Request delete many playlist item [Authorize]
```
  DELETE /api/WatchListItem/delete-many/{userId}
```
| Path parameters | Type     | Description                       |
| :--------  | :------- | :-------------------------------- |
| `userId`     | `int` | **Required** |

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