# Contribution and Versioning

## Versioning

Atlas follows semantic versioning:

`major.minor.patch`

* major = large new feature sets, breaking changes to existing interfaces or features
* minor = small features, minor tweaks to application logic
* patch = bugfixes, small documentation improvements

Only the *public interface* of ATLAS is versioned in code. In practice, this means two projects within this solution:

* `Atlas.Functions.PublicApi`
    * The public HTTP interface that consumers should use for all HTTP requests to ATLAS
* `Atlas.Client.Models`
    * All models expected to be needed by external consumers are within this package, which is versioned in-step with the public api.

## Releases

Atlas has no central host, so it is expected that any registry running a copy of the Atlas application will manage their own release pipeline.

From a centralised perspective, Atlas is not released as an application, nor published to a package feed as a code library.

Instead, stable versions of Atlas will be tagged in the following format:

`stable/x.y.z`, where `x.y.z` is the [version](#versioning) of Atlas.

Maintainers of Atlas instances should aim to only deploy from such tags - other commits may exist on the `master` git branch, but will not have been
fully tested or signed off.

In order to "release" code in this manner, someone at Anthony Nolan should sign off on a batch of changes to be released.

This sign-off process should involve ensuring:

* All automated tests have passed
* Manual testing of the algorithm has been performed, if deemed necessary
* If algorithmic logic has changed, an expert in the field of HLA matching must sign off the algorithmic changes.
* Documentation has been updated as appropriate:
    * Notably, the [Feature CHANGELOG](./Atlas.Functions.PublicApi/CHANGELOG_Atlas.md) should always be updated, and
      the [Client CHANGELOG](./Atlas.Client.Models/CHANGELOG_Client.md) should always be updated if any client models were changed


## Contributing

To contribute to Atlas, the following steps should be taken: 

* Ensure there is an appropriate Github Issue for the change being made - if one does not exist, please create one with a description of why the change is necessary
* Fork the repository, and make any code changes on a branch of your fork
* Create a Github Pull Request from your fork branch to the master branch of this repository
* All code changes will then be reviewed by a code owner before they can be merged into the master branch
* If any testing fails post-merge, you may be asked by a repository owner to assist in fixes as required.  