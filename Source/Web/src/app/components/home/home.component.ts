import { Component } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http'  
  
import { Uploader } from '../../../entities/uploader';  
import { UploadQueue } from '../../../entities/uploadqueue'; 
import { ApiService } from '../../../services/api-service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  //getter : get overall progress  
  // get progress(): number {  
  //   let psum = 0;  
  
  //   for (let entry of this.uploader.queue) {  
  //     psum += entry.progress;  
  //   }  
  
  //   if (psum == 0)  
  //     return 0;  
  
  //   return Math.round(psum / this.uploader.queue.length);  
  // };  
  public message: string;  
  public uploader: UploadQueue ;  
  
  constructor(private http: HttpClient, private _apiService: ApiService) {  
    this.message = '';        
  }  
  
  // onFilesChange(fileList: Array<File>) {  
  //   for (let file of fileList) {  
  //     this.uploader.queue.push(new UploadQueue(file));  
  //   };    
  // }  
  
  onFileInvalids(fileList: Array<File>) {  
    //TODO handle invalid files here  
  }  
    
  onSelectChange(event: EventTarget) {  
    let eventObj: MSInputMethodContext = <MSInputMethodContext>event;  
    let target: HTMLInputElement = <HTMLInputElement>eventObj.target;  
    let files: FileList = target.files;  
    let file = files[0];  
    if (file) {  
      this.uploader = new UploadQueue(file);  
      console.log(this.uploader);  
        
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
  
        const uploadReq = new HttpRequest('POST', `https://localhost:44310/api/accountperiodbalance`, formData, {  
          reportProgress: true,  
        });    
        
        this._apiService.uploadFile(uploadReq)
        //this._apiService.post('accountperiodbalance',formData )
        .subscribe(res => {
          if (res.type === HttpEventType.UploadProgress) {             
                selectedFile.progress = Math.round(100 * res.loaded / res.total);  
                console.log(selectedFile.progress);
              }  
              else if (res.type === HttpEventType.Response)  
                selectedFile.message = res.body.toString(); 
        },
          err => {
            console.error(`Error occured retrieving resource canlendar ${err}`);
          });

        // this.http.request(uploadReq).subscribe(event => {  
        //   if (event.type === HttpEventType.UploadProgress) {             
        //     selectedFile.progress = Math.round(100 * event.loaded / event.total);  
        //   }  
        //   else if (event.type === HttpEventType.Response)  
        //     selectedFile.message = event.body.toString();  
        // });  
      }  
  }  
  //upload all selected files to server  
  // uploadAll() {  
  //   //find the remaning files to upload  
  //   let remainingFiles = this.uploader.queue.filter(s => !s.isSuccess);  
  //   for (let item of remainingFiles) {  
  //     this.upload(item.id);  
  //   }  
  // }  
  
  // cancel all   
  cancelAll() {  
    //TODO  
  } 
}
