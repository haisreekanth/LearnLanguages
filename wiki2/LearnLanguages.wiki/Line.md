-
### Definition:
A Line is a phrase with a line number.

### Notes:
* A Line does not have an inherent language. It is just has a Phrase (which has a language).
* A Line has a many-to-many relationship to MLTs. It can be a part of many MLTs, and MLTs can have many lines. But an MLT can't contain the same line twice, as a line contains its own line number. I haven't put in this constraint in the DB. I'm not sure how I would in the DB, just controlling it programmatically on the other side.  I'll do this if the need arises. 