-
## Objective
###To create a superior Language Learning program, composed of pluggable pieces that compete with each other to maximize learning efficiency. 

## Problem Domain Questions
### Who gets to pick an item to present to the student? 
Teacher Ms. Johnson? Usually the teacher chooses the questions to ask (stimulus to present). Usually, this is from a set already created by author(s) of book(s), or other teachers, combined with that specific teacher.  But in the end, it comes down to the teacher to pick and choose.

### What gets presented to the user?
A question? A quiz? A video? A presentation? Someone's notes?...The current best student's notes, perhaps? A motivation picture? Vocab Card #5? Vocab Card #285? 

### Where does the item get presented to the user?
Main area? Pop-up Window? Thought bubble? Dancing light bulbs around edge? 

### When does an item get presented to the user?
Every 10 seconds? Depends on item? As needed, until all questions are answered?

## Relating Problem Domain to Code Architecture
### There is "One Meta-Pattern" that all other patterns throughout the code can always be related/transformed into.

I think of this as being like a fractal.  You have "one" shape, and everywhere you look, when you zoom in, you realize that that particular shape contains that "one" shape throughout, though in different variations.  But what does this mean in more concrete terms - in terms of actual coding?

Who does what? When? How? These can be encapsulated in a "living" system of competition, at the heart of which is the Exchange of information.  This is why I'm using an Exchange Pattern at the heart of the program.  Before I get to the details of my particular Exchange Pattern variant, however, I want to say at a higher level what I want this to accomplish.

## Organic Component Upgrading

Ideally, if I break down all of my systems into pieces, and I rate each individual piece, then I should be able to swap out other, better pieces for each individual piece.  This is the thought process behind pluggable components, dependency injection, factory patterns...the list goes on and on.  But when we "upgrade" a component, how do we know we are really UP-grading? How many times have you heard, "Oh, I hate the new look."? While others say, about the exact same thing, "Oh, this new upgrade is AWESOME!!!!!"?  

As best as I can tell, the way it currently works is almost strictly Top-Down, with some manual Bottom-Up feedback.  By this, I mean that at the "Top" (management), some set of people decide what they want to happen, and then communicate those ideas to the coders (via middle-management). The coders then write the code, and the end product is born out of the ideals of the Top, and this production cycle continues until the proper look is achieved. There is some feedback from the user, provided to some data-collecting entity that is in contact with the Top.  This data then changes the Top's ideals, which then get propagated down the chain to the coders, who then make the changes.  This is just a general look at it, and the Top doesn't have to be management in a corporation - this applies also to Open-Source projects as well.  The point is that there is some ideal look/function that is delegated by some central entity.

My long-term vision is to have Organically Upgraded Components throughout almost the entire system, with a drastically reduced centrality to it.  Specifically, what I envision is not to have version upgrades as we do, but have upgrades "injected" into an economical exchange system, and then see if the injection "takes".  This would create a more piece-wise, "organic" look, whose end-goal is to provide user satisfaction, taken from user metrics.  I know there are drawbacks to this approach, but it is nonetheless the one I am taking, as I think there is significant opportunity to be had...of course, easier said than done!

So, I bring us back to what I consider to be the heart of the architecture: IExchange, and my Exchange Pattern Variant

## My Exchange Pattern: IExchange

The IExchange interface uses a pub-sub pattern (PUBlish-SUBscribe), that entails 4 parts: Opportunity, Offer, Offer Response, Status Update. This is a top-down control pattern, essentially mirroring a preemptive OS. The "top" is what is "controlling" what is presented to the user, but it does this in an economic (as in monetary system of exchange economic) way.  The bottom-up portion will come into play when we start combining multiple IExchange implementations, who themselves meet in an Exchange (Meta-Exchange Pattern).  To start with, however, we have an Exchange Singleton implementation of IExchange.  

So, the four parts are Opportunity, Offer, Offer Response, and Status Update.  The idea is that whoever uses the Exchange controls some resource.  Right now, with my Exchange Singleton, the resource is access to the UI.  Concretely, I have a StudyASongViewModel that has a "Go" button on it.  When the user clicks Go, the ViewModel creates an Opportunity, containing the job info.  

This info includes:
1. The Target, a MultiLineTextList (a list of songs, as a song is a MultiLineText).
2. The ViewModel interface (IViewModelBase) of which the end-product will be an implementation,   
3. Some extra information about the target and job, including the user's native language, and an expiration for the job, and the precision it expects to be considered knowledge. (Knowledge is considered in terms of a probability of recall, so above the threshold, and item is considered to be "known", and below, "unknown")

So, this container IViewModelBase is the resource that the ViewModel owns.  And basically, the StudyASongViewModel publishes a message saying, "Hey, I have this list of songs, and I want someone to produce an IViewModelBase for me, with no expiration date on the job, and I'm including some additional info like how precise I want it to be, and here is the consumer's native language.  Give me the ViewModel." 

It does this by creating this Opportunity and publishing it on the Exchange.  

Now, we have a StudyPartner that is subscribed to the Exchange for Opportunities of just this kind.  So, it hears the message on the Exchange, and it creates an Offer.  It publishes this Offer on the Exchange, and the Offer essentially says, "Hey, I'd like to do that Opportunity with ID XXX.  Here are my details."

The StudyASongViewModel listens to this Offer message through the Exchange, and it doesn't even have to acknowledge that it got the Offer.  I want to point out that this is top-down control.  It has the opportunity to completely ignore those who give them Offers that it doesn't care about.  It can publish an OfferResponse, but it doesn't have to.  As it is, it does create the OfferResponse, and publishes this on the Exchange.

The StudyPartner then hears the OfferResponse on the Exchange, and that the response is the go-ahead.  So, the StudyPartner begins it's study session and produces a single container ViewModel (and thus View) that it will populate with it's own sub-ViewModels, however it wants.  Note here that the publisher of the Opportunity, however, retains full access to the container ViewModel and can get rid of it whenever it likes.  Again, this is top-down control, just as a preemptive OS can interrupt a process, and deny that process continued processing time (resource) for as long as it wants.

## Components Compete Alongside Problem-Domain Decisions

The factors going into learning include both the presentation of the material (who, what, when, how) and the material's inherent difficulty relative to the user's abilities.  So how good a component is is just as much of a factor as much as how well the material is organized.  Actually, saying it's "just as much of a factor" isn't strictly accurate.  If we have components competing together with the material organization, measured with standardized metrics (performance metrics), then we can create a neural network to measure the strengths of the various components, _and have the system tell us how important each aspect is_.  

This means that we will have the components themselves competing alongside the arrangement of components.  And this is obviously what happens anyway, right?  Current learning systems, however, are disconnected and disparate.  But why not create an open Exchange system, wherein the components (and their upgrades) compete with and alongside the material structure itself?

The program will start with a default study partner, that has relatively rigid algorithms, that will build up some sort of neural network based on the information it accumulates from usage.  Eventually, through hopefully continued support and success, the program grows and adds functionality.  If we have a solid enough foundation, then the migratory process from V1 to V1.1 (feature versioning, not necessarily bug-fixing) happens can happen from within the context of the initial V1 competition system.  The material created for V1 components has some ability to try to grow alongside new components, when available, and thus V1 components are competing with V1.1 components.  If the new components really do make a difference, then they will survive.  But factors like, how much a user like/dislikes changes will affect its survival.

Let me give a concrete example.  We have a question/answer ViewModel/View.  Right now, actually, I have two: Manual and Timed.  So these are competing with each other.  But what about a component that shows random videos?  That would be cool right?  Let's see how that affects the learning process.  We introduce a new component into the learning system.  When it is shown to the user, it adds to the user feedback area something like "Do you like this new gizmo?"  They say they like it OK, so we keep testing it out.  If they hate it, then we drop its value so much that it becomes improbable (but still possible) to survive.  So, now, we are presenting material with three different types of ViewModels/Views: ManualQA, TimedQA, TimedQAWithVideo.  We now measure against our same metrics, and depending on the results, we weight these presentation configurations accordingly.  

This is over just one user.  Across multiple users, we can take into account their specific learning strategies involved, combined with different user capability metrics.  The ability for maximizing learning efficiency should hopefully be apparent here.

to be cont'd