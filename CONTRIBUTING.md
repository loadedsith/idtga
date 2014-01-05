We love pull requests. Here's a quick guide:

1. Fork the repo. Name if it is a bug fix for a documented issue, name the repo Bugfix/Issue-[issue #].

2. Please test your code thoroughly. As of this writing there are not yet any tests, when there are please be sure to run them first.

3. If you can, add a test for your change.

4. Push to your fork and submit a pull request.


At that point you're waiting on peer test. The peer will pull a clean copy of 'Integration' branch, pull your patch in to a local branch, build, and test as much as they deem relevant. Ideally we would  like to at least comment on, if not fully peer test, pull requests ASAP. We may suggest some changes or improvements or alternatives. 

Some things that will increase the chance that your pull request is accepted, [slightly modified] from the Ruby on Rails guide:

* Use descriptive variable names and comment, comment, comment.
* A good test is one that fails without your code, and passes with it.
* Update the documentation, the surrounding one, examples elsewhere, guides,
  whatever is affected by your contribution

Syntax:

* Two spaces, no tabs.
* No trailing whitespace. Blank lines should not have any space.
* A variable or object's type may change. Name them reflect contents, not types: var messagesCache = new Object(); not messagesObject = new Object(); 
* a = b and not a=b.
* Follow the conventions you see used in the source already.

And in case we didn't emphasize it enough: we need more tests!