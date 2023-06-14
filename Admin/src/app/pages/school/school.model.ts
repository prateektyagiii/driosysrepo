export interface Table {
    
         SchoolId: number,
          Name:string,
          StreetAddress1: string,
          StreetAddress2: string,
          City: string,
          State: string,
          Zipcode: string,
          Country: string
          Latitude:string,
          Longitude: string,
        //   CreatedBy: string,
        //   CreatedDate: Date,
        //   UpdatedBy: string,
        //   UpdatedDate: Date,
        //   IsActive:boolean,
        //   IsDeleted: boolean
     
}

// Search Data
export interface SearchResult {
    tables: Table[];
    total: number;
}

// export interface Customers {
//     name: string;
//     email: string;
//     phone: string;
//     balance: string;
//     date: string;
// }
