WindowsPhoneProject-SignalR
===========================
Windows phone Chat application based on GeoLocation.



## Messaging protocol
Messages between server and client are sent via SignalR. The following messages are documented:

#### myLocation
```
location
{
  latitude: 52.3702160
  longitude: 4.8951680
}
```

#### startConversation
```
conversation
{
  user: 12
}
```

#### sendMessage
```
message
{
  user_id: 123
  message: "Hello user 132!"
}
```
