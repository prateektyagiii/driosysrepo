export interface Table {
    
    Userid: number,
    ProfileImg:string,
    Firstname: string,
    Middlename: string,
    Lastname: number,
    Gender: string,
    UserEmail: string,
    UserPassword: string,
    Education:string,
    Bio: string,
    Role: number,
    SchoolId: number,
    IsAdmin : number
}

// Search Data
export interface SearchResult {
tables: Table[];
total: number;
}