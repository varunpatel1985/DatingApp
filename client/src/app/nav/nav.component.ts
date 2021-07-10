import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model:any ={}


  constructor(public accountService:AccountService, private router:Router, private toastr: ToastrService) { 

  }

  ngOnInit(): void {
   
    console.log("accountService.currentUser$",this.accountService.currentUser$.source);
  }

 
  login(){
    console.log(this.model);

    this.accountService.login(this.model).subscribe(response=>{
     this.router.navigateByUrl('/members');
      console.log("After login",response);
      console.log("accountService.currentUser$",this.accountService.currentUser$);
      
    },error=>{
      console.log(error)
    this.toastr.error(error.error)
    });
    
  }

  logout()
  {

    this.accountService.logout();
    this.router.navigateByUrl('/');
   
  }


}
