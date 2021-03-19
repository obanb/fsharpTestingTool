namespace helloworld.Issue

module types =
    open System.Collections.Generic
    open MongoDB.Bson
    open MongoDB.Bson.Serialization.Attributes

    type Issue =
        { [<BsonId>]
          [<BsonRepresentation(BsonType.ObjectId)>]
          Id: string
          desc: string
          name: string
          content: string }

    type IssueFilter = | All

    type IssueFind = IssueFilter -> IEnumerable<Issue>

    type CreateIssueInput =
        { name: string
          content: string
          desc: string }
