namespace helloworld.Common.Database

module connections =
    open MongoDB.Driver

    let getInternalMongoClient () =
        let conn =
            MongoClient("mongodb+srv://admin:clovek789@cluster0.6tq4i.mongodb.net/cloud?retryWrites=true&w=majority")

        conn.GetDatabase "cloud"