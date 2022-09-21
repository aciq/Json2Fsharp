namespace MyNamespace


open System
open System.Collections.Generic
open System.Text.Json

[<CLIMutable>]
type Entity = {
    /// e.g. "FullContact, Inc"
    name : string
    /// e.g. 2010
    founded : int
    /// e.g. 350
    employees : int
}

[<CLIMutable>]
type Locale = {
    /// e.g. "en"
    code : string
    /// e.g. "English"
    name : string
}

[<CLIMutable>]
type Category = {
    /// e.g. "OTHER"
    code : string
    /// e.g. "Other"
    name : string
}

[<CLIMutable>]
type Industry = {
    /// e.g. "SIC", "NAICS"
    ``type`` : string
    /// e.g. "Computer Programming, Data Processing, and Other Computer Related Services", "Data Processing, Host
    name : string
    /// e.g. "737", "5182"
    code : string
}

[<CLIMutable>]
type Email = {
    /// e.g. "support@fullcontact.com", "team@fullcontact.com", "sales@fullcontact.com"
    value : string
    /// e.g. "other", "sales", "work"
    label : string
}

[<CLIMutable>]
type Phone = {
    /// e.g. "+1 (720) 475-1292", "+1 (888) 330-6943", "(303) 717-0414"
    value : string
    /// e.g. "other"
    label : string
}

[<CLIMutable>]
type Twitter = {
    /// e.g. "fullcontact"
    username : string
    /// e.g. "https://twitter.com/fullcontact"
    url : string
    /// e.g. "twitter"
    service : string
    /// e.g. 2873
    followers : int
    /// e.g. 120
    following : int
}

[<CLIMutable>]
type Linkedin = {
    /// e.g. "fullcontact-inc-"
    username : string
    /// e.g. "https://www.linkedin.com/company/fullcontact-inc-"
    url : string
    /// e.g. "linkedin"
    service : string
}

[<CLIMutable>]
type Profiles = {
    /// e.g. {"username":"fullcontact","url":"https://twitter.com/fullcontact","service":"twitter","followers":28
    twitter : Twitter
    /// e.g. {"username":"fullcontact-inc-","url":"https://www.linkedin.com/company/fullcontact-inc-","service":"
    linkedin : Linkedin
}

[<CLIMutable>]
type Location = {
    /// e.g. "Headquarters"
    label : string
    /// e.g. "1755 Blake Street"
    addressLine1 : string
    /// e.g. "Suite 450"
    addressLine2 : string
    /// e.g. "Denver"
    city : string
    /// e.g. "Colorado"
    region : string
    /// e.g. "CO"
    regionCode : string
    /// e.g. "80202"
    postalCode : string
    /// e.g. "United States"
    country : string
    /// e.g. "USA"
    countryCode : string
    /// e.g. "1755 Blake Street Suite 450 Denver CO 80202"
    formatted : string
}

[<CLIMutable>]
type Image = {
    /// e.g. "https://d2ojpxxtu63wzl.cloudfront.net/...01842912b", "https://d2ojpxxtu63wzl.cloudfro...33e489c3aa0
    url : string
    /// e.g. "logo", "other"
    label : string
}

[<CLIMutable>]
type Url = {
    /// e.g. "https://www.fullcontact.com", "https://www.fullcontact.com/feed", "https://www.youtube.com/watch?v=
    value : string
    /// e.g. "website", "rss", "youtube"
    label : string
}

[<CLIMutable>]
type Global = {
    /// e.g. 40093
    rank : int
    /// e.g. "Global"
    name : string
}

[<CLIMutable>]
type Us = {
    /// e.g. 15545
    rank : int
    /// e.g. "United States"
    name : string
}

[<CLIMutable>]
type CountryRank = {
    /// e.g. {"rank":40093,"name":"Global"}
    ``global`` : Global
    /// e.g. {"rank":15545,"name":"United States"}
    us : Us
}

[<CLIMutable>]
type In = {
    /// e.g. 29313
    rank : int
    /// e.g. "India"
    name : string
}

[<CLIMutable>]
type Gb = {
    /// e.g. 18772
    rank : int
    /// e.g. "United Kingdom"
    name : string
}

[<CLIMutable>]
type LocaleRank = {
    /// e.g. {"rank":29313,"name":"India"}
    ``in`` : In
    /// e.g. {"rank":18772,"name":"United Kingdom"}
    gb : Gb
    /// e.g. {"rank":15545,"name":"United States"}
    us : Us
}

[<CLIMutable>]
type Traffic = {
    /// e.g. {"global":{"rank":40093,"name":"Global"},"us":{"rank":15545,"name":"United States"}}
    countryRank : CountryRank
    /// e.g. {"in":{"rank":29313,"name":"India"},"gb":{"rank":18772,"name":"United Kingdom"},"us":{"rank":15545,"
    localeRank : LocaleRank
}

[<CLIMutable>]
type KeyPeople = {
    /// e.g. "Bart Lorang", "Scott Brave"
    fullName : string
    /// e.g. "CEO", "CTO"
    title : string
    /// e.g. "https://d2ojpxxtu63wzl.cl...bf84a60c5926701b4c5033", "https://d2ojpxxtu63wzl.cloudf....49ec3bcbb401
    avatar : string
}

[<CLIMutable>]
type Details = {
    /// e.g. {"name":"FullContact, Inc","founded":2010,"employees":350}
    entity : Entity
    /// e.g. [{"code":"en","name":"English"}]
    locales : List<Locale>
    /// e.g. [{"code":"OTHER","name":"Other"}]
    categories : List<Category>
    /// e.g. [{"type":"SIC","name":"Computer Programming, Data Processing, and Other Computer Related Services","
    industries : List<Industry>
    /// e.g. [{"value":"support@fullcontact.com","label":"other"},{"value":"team@fullcontact.com","label":"sales"
    emails : List<Email>
    /// e.g. [{"value":"+1 (720) 475-1292","label":"other"},{"value":"+1 (888) 330-6943","label":"other"},{"value
    phones : List<Phone>
    /// e.g. {"twitter":{"username":"fullcontact","url":"https://twitter.com/fullcontact","service":"twitter","fo
    profiles : Profiles
    /// e.g. [{"label":"Headquarters","addressLine1":"1755 Blake Street","addressLine2":"Suite 450","city":"Denve
    locations : List<Location>
    /// e.g. [{"url":"https://d2ojpxxtu63wzl.cloudfront.net/...01842912b","label":"logo"},{"url":"https://d2ojpxx
    images : List<Image>
    /// e.g. [{"value":"https://www.fullcontact.com","label":"website"},{"value":"https://www.fullcontact.com/fee
    urls : List<Url>
    /// e.g. ["CRM","Contact Management","Developer APIs","Information Services","Social Media"]
    keywords : List<string>
    /// e.g. {"countryRank":{"global":{"rank":40093,"name":"Global"},"us":{"rank":15545,"name":"United States"}},
    traffic : Traffic
    /// e.g. [{"fullName":"Bart Lorang","title":"CEO","avatar":"https://d2ojpxxtu63wzl.cl...bf84a60c5926701b4c503
    keyPeople : List<KeyPeople>
}

[<CLIMutable>]
type DataAddOn = {
    /// e.g. "keypeople"
    id : string
    /// e.g. "Key People"
    name : string
    /// e.g. true
    enabled : bool
    /// e.g. true
    applied : bool
    /// e.g. "People of interest at this company"
    description : string
    /// e.g. "https://fullcontact../api-next-slate/#keypeople"
    docLink : string
}

[<CLIMutable>]
type Root = {
    /// e.g. "FullContact, Inc."
    name : string
    /// e.g. "1755 Blake Street, Suite 450, Denver, Colorado, United States"
    location : string
    /// e.g. "@fullcontact"
    twitter : string
    /// e.g. "https://www.linkedin.com/company/fullcontact-inc-"
    linkedin : string
    /// e.g. "FullContact is the ... contacts and be awesome with people."
    bio : string
    /// e.g. "https://d2ojpxxtu63wzl.cloudfront.net/stati...8ea9e4d47f5af6c"
    logo : string
    /// e.g. "https://www.fullcontact.com"
    website : string
    /// e.g. 2010
    founded : int
    /// e.g. 350
    employees : int
    /// e.g. "en"
    locale : string
    /// e.g. "Other"
    category : string
    /// e.g. {"entity":{"name":"FullContact, Inc","founded":2010,"employees":350},"locales":[{"code":"en","name":
    details : Details
    /// e.g. [{"id":"keypeople","name":"Key People","enabled":true,"applied":true,"description":"People of intere
    dataAddOns : List<DataAddOn>
    /// e.g. "2017-11-01"
    updated : string
}

