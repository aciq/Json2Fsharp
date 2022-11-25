namespace NsName

open System

type awesomeobject =
    { SomeProps1: int
      SomeProps2: string }

    static member Default: awesomeobject = { SomeProps1 = 0; SomeProps2 = "" }

type user =
    { id: int
      name: string
      created_at: string
      updated_at: string
      email: string
      testanadditionalfield: string }

    static member Default: user =
        { id = 0
          name = ""
          created_at = ""
          updated_at = ""
          email = ""
          testanadditionalfield = "" }

type Class1 =
    { id: int
      user_id: string
      awesomeobject: awesomeobject
      created_at: string
      updated_at: string
      users: user list }

    static member Default: Class1 =
        { id = 0
          user_id = ""
          awesomeobject = awesomeobject.Default
          created_at = ""
          updated_at = ""
          users = [] }

type Class2 =
    { SomePropertyOfClass2: string }

    static member Default: Class2 = { SomePropertyOfClass2 = "" }

type Root =
    { Class1: Class1
      Class2: Class2 }

    static member Default: Root =
        { Class1 = Class1.Default
          Class2 = Class2.Default }


          
