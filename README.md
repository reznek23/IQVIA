# IQVIA

Application for IQVIA API. Contains base library for tweets loading, simple console and web (ASP.NET Core) apps, test project prototype.

DESCRIPTION:

Problem
A Client needs to pull 2 years of tweets they have collected and stored themselves. We need to make sure that we get all tweets tweeted in 2016 and 2017. The only way to get this data is to use the https://badapi.iqvia.io/swagger/.

Complications
* The API only lets you search for tweets with timestamps falling in a window specified by 'startDate' and 'endDate'.
* The API can only return 100 results in a single response, even if there are more than 100 tweets in the specified window.
* There is no indication that there are more results and there are no pagination parameters returned.
* There can be no duplicate entries in the results you return.

TO DO:

1) add docker 
2) add parallel loading
