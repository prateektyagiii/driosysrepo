

export interface Table {
    EventId: BigInteger;
    Name: string;
    Description: string;
    Userid: BigInteger;
    InterestId:BigInteger;
    PublicEvent:boolean;
    Capacity: bigint;
    EventTime: Date;
    StreetAddress1: string;
    StreetAddress2: string;
    City: string;
    State: string;
    Zipcode:string;
    Country: string;
    Latitude: number;
    Longitude: string;
    IsActive: boolean;
	IsDeleted: boolean; 

}

// Search Data
export interface SearchResult {
    tables: Table[];
    total: number;
}