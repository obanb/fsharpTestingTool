namespace helloworld.Common

module error =
    type ApplicationError =
        | NetworkError
        | Non200Response
        | DatabaseError of string
        | ParseError of string