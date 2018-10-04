import { Component } from '@angular/core';
import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http'
import { Uploader } from '../../../entities/uploader';
import { ApiService } from '../../../services/api-service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent {

    public uploader: Uploader;

    constructor(private http: HttpClient, private _apiService: ApiService) { }

    onSelectChange(event: EventTarget) {
        let eventObj: MSInputMethodContext = <MSInputMethodContext>event;
        let target: HTMLInputElement = <HTMLInputElement>eventObj.target;
        let files: FileList = target.files;
        let file = files[0];
        let extension = file.name.substring(file.name.lastIndexOf(".") + 1);

        if (file.size / 1024 / 1024 > 2) {
            alert('1MB exceeds');
        }
        if (extension === 'xlsx' || extension === 'txt' || extension === 'xlsm') {
            if (file) {
                this.uploader = new Uploader(file);                
            }
        }
        else {
            alert('invalid extension');
        }
    }

    // upload   
    upload() {
        if (this.uploader.id == null)
            return;

        let selectedFile = this.uploader;
        if (selectedFile) {
            const formData = new FormData();
            formData.append(selectedFile.file.name, selectedFile.file);

           // this._apiService.uploadFile('accountperiodbalance', formData)
            this._apiService.post('accountperiodbalance', formData)
              .subscribe(res => {
                if (res.type === HttpEventType.UploadProgress) {
                  // selectedFile.progress = Math.round(100 * res.loaded / res.total);
                  // console.log(selectedFile.progress);
                }
              },
                err => {
                  console.error(`Error occured retrieving resource canlendar ${err}`);
                });

            // this._apiService.upload('accountperiodbalance', this.uploader.file)
            // .subscribe(
            //   event => {
            //     if (event.type == HttpEventType.UploadProgress) {
            //       const percentDone = Math.round(100 * event.loaded / event.total);
            //       // console.log(`File is ${percentDone}% loaded.`);
            //       //this.progress = percentDone;
            //     } else if (event instanceof HttpResponse) {
            //      // this.final = 100;
            //     }
            //   },
            //   (err) => {
            //    // this.toaster.error("Error");
            //     return false;
            //   }, () => {
            //     // this.toaster.success(this.translateLabels['uploadSuccess']);
            //     //this.toaster.success("Uploaded Successfully !")
            //     return true;
            //   });

        }
    }

    // cancel all   
    cancelAll() {
        this.uploader = undefined;
    }
}
