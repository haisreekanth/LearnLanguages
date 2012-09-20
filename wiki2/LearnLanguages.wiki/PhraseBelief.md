-
### Definition:
A PhraseBelief contains information about a belief of a phrase. 

### Purpose
This represents an open-ended, abstract sense of some metadata on a phrase, with a strength, while also keeping track of the source (believer that holds this belief) and a relevant presenter (ReviewMethodId) to the user.

### Key Properties: 
* TimeStamp
* Text
* Strength (double)
* BelieverId
* ReviewMethodId

### Design Pros
* Open-ended
* Contains ability to store both text and numbers.

### Design Cons
* Lacks constraints as it stands.
  * In the future, I may decide to make this a more concrete entity, but for now this is what we got.

### Related
[Phrase Belief System](https://github.com/bill-mybiz/LearnLanguages/wiki/Phrase-Belief-System)