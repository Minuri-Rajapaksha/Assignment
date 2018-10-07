import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Uploader } from '../../../entities/uploader';
import { ApiService } from '../../../services/api-service';
import { Period } from '../../../entities/period';
import API from '../../../services/api-config.json';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

    uploader: Uploader;
    periodList: Period[];
    selectedPeriod: number;
    uploadOk = '';

    constructor(private http: HttpClient, private _apiService: ApiService) { }

    ngOnInit() {
        this.getAllPeriods();
    }

    getAllPeriods() {
        this._apiService.get(API.period.getDropdownList)
            .subscribe(res => {
                this.periodList = res;
            },
                err => {
                    console.error(`Error occured retrieving resource canlendar ${err}`);
                });
    }

    setPeriod(periodId: number) {
        this.selectedPeriod = periodId;
    }

    onSelectChange(event: EventTarget) {
        const eventObj: MSInputMethodContext = <MSInputMethodContext>event;
        const target: HTMLInputElement = <HTMLInputElement>eventObj.target;
        const files: FileList = target.files;
        const file = files[0];
        const extension = file.name.substring(file.name.lastIndexOf('.') + 1);

        if (file.size / 1024 / 1024 > 2) {
            alert('1MB exceeds');
        }
        if (extension === 'xlsx' || extension === 'txt' || extension === 'xlsm') {
            if (file) {
                this.uploader = new Uploader(file);
            }
        } else {
            alert('invalid extension');
        }
    }

    // upload
    upload() {
        this.uploadOk = undefined;
        if (this.uploader.id == null) {
            return;
        }
        if (this.selectedPeriod === undefined) {
            alert('Please select period');
            return;
        }

        const selectedFile = this.uploader;
        if (selectedFile) {
            const formData = new FormData();
            formData.append(selectedFile.file.name, selectedFile.file);
            formData.append('PERIOD', this.selectedPeriod.toString());

            // this._apiService.uploadFile('accountperiodbalance', formData)
            this._apiService.post(API.fileupload, formData)
                .subscribe(res => {
                    this.uploadOk = 'ok';
                    alert(res);
                    this.uploader = undefined;
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
