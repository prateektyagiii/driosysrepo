export interface Table {
    
    EventId: number,
     Name:string,
     Description: string,
     EventCreatedName: string,
     InterestId: number,
     PublicEvent: boolean,
     Capacity: number,
     EventTime: Date,
     StreetAddress1:string,
     StreetAddress2: string,
     City: string,
     State: string,
     Zipcode: string,
     Country: string,
     Latitude:number,
     Longitude: number
     ParticipatedId:number,
     Status:boolean,
     ParticipateName:string
     
}

// Search Data
export interface SearchResult {
tables: Table[];
total: number;
}