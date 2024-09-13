export interface UpdateContact{
    contactId:number;
    firstName:string;
    lastName:string;
    image:string;
    email:string;
    contactNumber:string;
    address:string;
    gender:string;
    favourite:boolean;
    countryId:number;
    stateId:number|null;
    imageByte:string;
    birthDate:string;

}