import { Injectable } from '@angular/core';
import * as $ from 'jquery';
@Injectable()
export class ToasterService {
   public showSnackbar(message:string) {
        $('#snackbar').html(message);
        $('#snackbar').addClass("show");
        
        setTimeout(function () { $('#snackbar').removeClass("show");}, 3000);
    }
}