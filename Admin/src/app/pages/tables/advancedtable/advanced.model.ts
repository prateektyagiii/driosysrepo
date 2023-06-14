// Table data
export interface Table {
    
    InterestId :  BigInteger;
    Name : string;
	Category : string;
    Icon : string;
    Description : string;
    CreatedBy : string;
    CreatedDate : Date ;
    UpdateBy : string;
    UpdatedDate : Date ;
    IsActive : boolean;
    IsDeleted : boolean;
	
}
// Search Data
export interface SearchResult {
    tables: Table[];
    total: number;
}
