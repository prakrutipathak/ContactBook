import { ContactCountry } from "./country.model";
import { ContactState } from "./state.model";

export interface Contact{
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
    stateId:number;
    imageByte:string;
    birthDate:string;
    country:ContactCountry;
    state:ContactState;

}