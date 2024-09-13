import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { AuthService } from 'src/app/service/auth.service';
import { ContactService } from 'src/app/service/contact.service';

@Component({
  selector: 'app-contact-list-pagination',
  templateUrl: './contact-list-pagination.component.html',
  styleUrls: ['./contact-list-pagination.component.css']
})
export class ContactListPaginationComponent {
  contacts:any[] | undefined |null;
  contactsForInitial: Contact[] = [];
  contactId:number |undefined;
  loading:boolean=false;
  searchQuery: string = '';
  pageNumber: number = 1;
  pageSize: number = 2;
  totalItems: number = 0;
  totalPages: number = 0;
  selectedLetter: string | null = null;
  isAuthenticated:boolean=false;
  letter: string='';
  sort:string="asc";
  username:string |null|undefined;
  uniqueFirstLetters: string[] = [];
  constructor(private contactservice:ContactService,private cdr:ChangeDetectorRef,private router: Router,private authService:AuthService){}
  ngOnInit(): void {
    this.loadContactsCount();
    this.loadAllContacts();
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.username=username;
      this.cdr.detectChanges();
     });
      }
      loadAllContacts(): void {
        this.loading = true;
        this.contactservice.getAllContacts().subscribe({
          next: (response: ApiResponse<Contact[]>) => {
            if (response.success) {
              console.log(response.data);
              this.contactsForInitial = response.data;
              this.updateUniqueFirstLetters();
            } else {
              console.error('Failed to fetch contacts', response.message);
            }
            this.loading = false;
          },
          error: (error) => {
            console.error('Error fetching contacts.', error);
            this.loading = false;
          }
        });
      }
      getUniqueFirstLetters(): string[] {
        // Extract first letters from contact names and filter unique letters
        const firstLetters = Array.from(new Set(this.contactsForInitial.map(contact => contact.firstName.charAt(0).toUpperCase())));
        return firstLetters.sort(); // Sort alphabetically
    }
      updateUniqueFirstLetters(): void {
        this.uniqueFirstLetters = this.getUniqueFirstLetters();
    }
      loadContactsCount(): void {
        if (this.letter|| this.searchQuery) {
          this.getAllContactsCountWithLetter(this.letter,this.searchQuery);
        
        } else {
          this.getAllContactsCountWithoutLetter();     
        }
      }
      getAllContactsCountWithLetter(letter:string,searchQuery:string):void{
        this.contactservice.getAllContactsCount(letter,searchQuery).subscribe({
          next: (response: ApiResponse<number>) => {
            if (response.success) {
              console.log(response.data);
              this.totalItems = response.data;
              this.totalPages = Math.ceil(this.totalItems / this.pageSize);
              this.getAllContactsWithLetter(letter,searchQuery);
            } else {
              console.error('Failed to fetch contacts count', response.message);
            }
            this.loading = false;
          },
          error: (error) => {
            console.error('Error fetching contacts count.', error);
            this.loading = false;
          }
        });

      }
      
      getAllContactsCountWithoutLetter():void{
        this.contactservice.getAllContactsCount(' ',' ').subscribe({
          next: (response: ApiResponse<number>) => {
            if (response.success) {
              console.log(response.data);
              this.totalItems = response.data;
              this.totalPages = Math.ceil(this.totalItems / this.pageSize);
              this.getAllContactsWithPagination();
            } else {
              console.error('Failed to fetch contacts count', response.message);
            }
            this.loading = false;
          },
          error: (error) => {
            console.error('Error fetching contacts count.', error);
            this.loading = false;
          }
        });

      }
      loadContacts(): void {
        this.loading = true;
        if (this.letter || this.searchQuery) {
          this.getAllContactsWithLetter(this.letter,this.searchQuery);
        } else {
          this.getAllContactsWithPagination();
        }
      }
      getAllContactsWithLetter(letter:string,searchQuery:string): void {
        this.loading = true;
        this.contactservice.getAllContactsWithPagination(this.pageNumber, this.pageSize, letter,this.sort,searchQuery).subscribe({
          next: (response: ApiResponse<Contact[]>) => {
            if (response.success) {
              console.log(response.data);
              this.contacts = response.data;
            } else {
              console.error('Failed to fetch contacts', response.message);
            }
            this.loading = false;
          },
          error: (error) => {
            console.error('Error fetching contacts.', error);
            this.loading = false;
          }
        });
      }
      getAllContactsWithPagination(): void {
        if (this.pageNumber > this.totalPages) {
          console.log('Requested page does not exist.');
          return;
        }
      
        this.contactservice.getAllContactsWithPagination(this.pageNumber, this.pageSize,' ',this.sort,'').subscribe({
          next: (response: ApiResponse<Contact[]>) => {
            if (response.success) {
              console.log(response.data);
              this.contacts = response.data;
            } else {
              console.error('Failed to fetch contacts', response.message);
            }
            this.loading = false;
          },
          error: (err) => {
            console.log(err);
            this.loading = false;
            alert(err.error.message);
          },
          complete: () => {
            this.loading = false;
            console.log('Completed');
          }
        });
      }
      
      filterByLetter(letter: string): void {
        if (this.isSelected(letter)) {
            this.letter = '';
        } else {
            this.letter = letter;
        }
        this.pageNumber = 1;
        this.loadContactsCount();
    }
    isSelected(letter: string): boolean {
      return this.letter === letter || (!this.letter && !letter);
  }
      changePageSize(pageSize: number): void {
        this.pageSize = pageSize;
        this.pageNumber = 1; 
        this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        this.loadContacts();
      }
      sortAsc()
  {
    this.sort = 'asc'
    this.pageNumber = 1;
    this.loadContacts();
  }
 
  sortDesc()
  {
    this.sort = 'desc'
    this.pageNumber = 1;
    this.loadContacts();
 
  }
  onSearch() {
    this.pageNumber = 1;
this.getAllContactsWithLetter(this.letter, this.searchQuery);
this.getAllContactsCountWithLetter(this.letter, this.searchQuery);
  }
  clearSearch() {
    this.searchQuery = '';
    this.pageNumber = 1;
    this.loadContactsCount();
  }
    
      changePage(pageNumber: number): void {
        this.pageNumber = pageNumber;
        this.loadContacts();
      }
    
      getContactImage(contact: Contact): string {
        if (contact.imageByte) {
            return 'data:image/jpeg;base64,' + contact.imageByte;
        } else {
            return 'assets/Defaultimage.png'; // Path to your default image
        }
    }
    
      confirmDelete(id:number):void{
        if(confirm('Are you sure?')){
          this.contactId = id;
          this.deleteContact();
        }
      }
      
      deleteContact():void{
        this.contactservice.deleteContact(this.contactId).subscribe({
          next:(response)=>{
            if(response.success){
              this.totalItems--;
              this.totalPages = Math.ceil(this.totalItems / this.pageSize);
              if (this.pageNumber > this.totalPages) {
                this.pageNumber = this.totalPages;
              }
              this.loadContacts();
            }else{
              alert(response.message);
            }
          },
          error:(err)=>{
            alert(err.error.message);
          },
          complete:()=>{
            console.log('completed');
          }
        })
        //this.router.navigate(['/contactpagination']);
      }

}
