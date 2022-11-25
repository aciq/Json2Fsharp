namespace NsName

open System

type entity =
    { name: string
      founded: int
      employees: int }

    static member Default: entity =
        { name = ""
          founded = 0
          employees = 0 }

type locale =
    { code: string
      name: string }
    static member Default: locale = { code = ""; name = "" }

type category =
    { code: string
      name: string }

    static member Default: category = { code = ""; name = "" }

type industry =
    { ``type``: string
      name: string
      code: int }

    static member Default: industry = { ``type`` = ""; name = ""; code = 0 }

type email =
    { value: string
      label: string }

    static member Default: email = { value = ""; label = "" }

type phone =
    { value: string
      label: string }

    static member Default: phone = { value = ""; label = "" }

type twitter =
    { username: string
      url: string
      service: string
      followers: int
      following: int }

    static member Default: twitter =
        { username = ""
          url = ""
          service = ""
          followers = 0
          following = 0 }

type linkedin =
    { username: string
      url: string
      service: string }

    static member Default: linkedin =
        { username = ""
          url = ""
          service = "" }

type profiles =
    { twitter: twitter
      linkedin: linkedin }

    static member Default: profiles =
        { twitter = twitter.Default
          linkedin = linkedin.Default }

type location =
    { label: string
      addressLine1: string
      addressLine2: string
      city: string
      region: string
      regionCode: string
      postalCode: int
      country: string
      countryCode: string
      formatted: string }

    static member Default: location =
        { label = ""
          addressLine1 = ""
          addressLine2 = ""
          city = ""
          region = ""
          regionCode = ""
          postalCode = 0
          country = ""
          countryCode = ""
          formatted = "" }

type image =
    { url: string
      label: string }

    static member Default: image = { url = ""; label = "" }

type url =
    { value: string
      label: string }

    static member Default: url = { value = ""; label = "" }

type ``global`` =
    { rank: int
      name: string }

    static member Default: ``global`` = { rank = 0; name = "" }

type us =
    { rank: int
      name: string }

    static member Default: us = { rank = 0; name = "" }

type countryRank =
    { ``global``: ``global``
      us: us }

    static member Default: countryRank =
        { ``global`` = { rank = 0; name = "" }
          us = us.Default }

type ``in`` =
    { rank: int
      name: string }

    static member Default: ``in`` = { rank = 0; name = "" }

type gb =
    { rank: int
      name: string }

    static member Default: gb = { rank = 0; name = "" }

type localeRank =
    { ``in``: ``in``
      gb: gb
      us: us }

    static member Default: localeRank =
        { ``in`` = { rank = 0; name = "" }
          gb = gb.Default
          us = us.Default }

type traffic =
    { countryRank: countryRank
      localeRank: localeRank }

    static member Default: traffic =
        { countryRank = countryRank.Default
          localeRank = localeRank.Default }

type keyPeople =
    { fullName: string
      title: string
      avatar: string }

    static member Default: keyPeople =
        { fullName = ""
          title = ""
          avatar = "" }

type details =
    { entity: entity
      locales: locale list
      categories: category list
      industries: industry list
      emails: email list
      phones: phone list
      profiles: profiles
      locations: location list
      images: image list
      urls: url list
      keywords: string list
      traffic: traffic
      keyPeople: keyPeople list }

    static member Default: details =
        { entity = entity.Default
          locales = []
          categories = []
          industries = []
          emails = []
          phones = []
          profiles = profiles.Default
          locations = []
          images = []
          urls = []
          keywords = []
          traffic = traffic.Default
          keyPeople = [] }

type dataAddOn =
    { id: string
      name: string
      enabled: bool
      applied: bool
      description: string
      docLink: string }

    static member Default: dataAddOn =
        { id = ""
          name = ""
          enabled = false
          applied = false
          description = ""
          docLink = "" }

type Root =
    { name: string
      location: string
      twitter: string
      linkedin: string
      bio: string
      logo: string
      website: string
      founded: int
      employees: int
      locale: string
      category: string
      details: details
      dataAddOns: dataAddOn list
      updated: System.DateTime }

    static member Default: Root =
        { name = ""
          location = ""
          twitter = ""
          linkedin = ""
          bio = ""
          logo = ""
          website = ""
          founded = 0
          employees = 0
          locale = ""
          category = ""
          details = details.Default
          dataAddOns = []
          updated = DateTime.MinValue }
