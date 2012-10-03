-
### Definition:
A MultiLineText (MLT) is any ordered group of lines. 

### Key Properties
* Title
* Lines
* AdditionalMetadata (string)
  * This I have included as a general query string for future expansion. I'm doing this instead of blowing up the number of different subclasses of this, each of which would require its own plumbing for little value.

### Notes:
* A Song is a MLT.
* A Poem is an MLT.
* A Script is an MLT.
* A Paragraph may end up being an MLT, but I am not 100% positive on this yet. I think it will be.
* A MLT does not have an inherent language. However, as I've written it for right now, it creates it by passing it ordered phrases, and each of the phrases has to be in the same language.