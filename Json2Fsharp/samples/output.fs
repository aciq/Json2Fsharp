namespace MyNamespace.Generated


open System
open System.Collections.Generic
open System.Text.Json

[<CLIMutable>]
type Awesomeobject = {
    /// e.g. 1
    SomeProps1 : int
    /// e.g. "test"
    SomeProps2 : string
}

[<CLIMutable>]
type User = {
    /// e.g. "6"
    id : string
    /// e.g. "Test Child 1"
    name : string
    /// e.g. "2015-06-02 23:33:90"
    created_at : string
    /// e.g. "2015-06-02 23:33:90"
    updated_at : string
    /// e.g. "test@gmail.com"
    email : string
    /// e.g. "tet"
    testanadditionalfield : string
}

[<CLIMutable>]
type Class1 = {
    /// e.g. 4
    id : int
    /// e.g. "user_id_value"
    user_id : string
    /// e.g. {"SomeProps1":1,"SomeProps2":"test"}
    awesomeobject : Awesomeobject
    /// e.g. "2015-06-02 23:33:90"
    created_at : string
    /// e.g. "2015-06-02 23:33:90"
    updated_at : string
    /// e.g. [{"id":"6","name":"Test Child 1","created_at":"2015-06-02 23:33:90","updated_at":"2015-06-02 23:33:90","email":"test@gmail.com"},{"id":"6","name":"Test Child 1","created_at":"2015-06-02 23:33:90","updated_at":"2015-06-02 23:33:90","email":"test@gmail.com","testanadditionalfield":"tet"}]
    users : List<User>
}

[<CLIMutable>]
type Class2 = {
    /// e.g. "SomeValueOfClass2"
    SomePropertyOfClass2 : string
}

[<CLIMutable>]
type Root = {
    /// e.g. {"id":4,"user_id":"user_id_value","awesomeobject":{"SomeProps1":1,"SomeProps2":"test"},"created_at":"2015-06-02 23:33:90","updated_at":"2015-06-02 23:33:90","users":[{"id":"6","name":"Test Child 1","created_at":"2015-06-02 23:33:90","updated_at":"2015-06-02 23:33:90","email":"test@gmail.com"},{"id":"6","name":"Test Child 1","created_at":"2015-06-02 23:33:90","updated_at":"2015-06-02 23:33:90","email":"test@gmail.com","testanadditionalfield":"tet"}]}
    Class1 : Class1
    /// e.g. {"SomePropertyOfClass2":"SomeValueOfClass2"}
    Class2 : Class2
}

