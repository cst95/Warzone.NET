# Warzone.NET
A C# wrapper around the Call of Duty API, primarily focussed on allowing users to fetch Warzone related data.

## Usage
### Setup
Accessing any of the endpoints requires the user to create a new `WarzoneClient` and then log in with a valid set of [Call of Duty](https://my.callofduty.com/mycod-home) credentials.
```
var client = new WarzoneClient();
var loginResult = await client.LoginAsync("MyEmailAddress@email.com", "MyPassword");
```
The `LoginAsync` method returns a bool indicating whether the login attempt was successful. If `true` then you are good to go with the remainder of the endpoints!

### Retrieving User Stats 
After logging in successfully you can proceed to use the other available methods that the `Warzone Client` provides.
For example, to fetch a summary of the user's last twenty games you can use the following snippet
```
var data = await client.GetLastTwentyWarzoneMatchesAsync("MyGamertag", Platforms.Xbox);
```
If your login session has expired this method will throw a `NotLoggedInException` and you will need to reauthenticate using the `LoginAsync` method.


## Current features
- Summarized data for the user's last twenty Warzone games - `GetLastTwentyWarzoneMatchesAsync` (Only standard BR gamemodes)
- Summarized data for the user's Warzone games between a start and end date `GetWarzoneMatchesAsync` (Only standard BR gamemodes)
