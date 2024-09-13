import { ContactCountry } from "./country.model";

export interface CountryState{
    stateId:number;
    stateName:string;
    countryId:number;
    country:ContactCountry
}