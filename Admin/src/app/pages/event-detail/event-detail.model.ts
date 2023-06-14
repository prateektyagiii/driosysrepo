export interface EventDetailModel {
    debugger
    Userid :Number;
    Name: string;
    CreatorImg:string;
    Description:string;
	EventTime : Date;
    EventCreatedName : string;
    PublicEvent : Boolean;
    totaluser : Number;
    users: Users[]
        
}

export interface Users 
    { ParticipatedId:Number;
        ParticipateName:string;
}
