import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Uploader } from '../../../entities/uploader';
import { ApiService } from '../../../services/api-service';
import { Period } from '../../../entities/period';
import API from '../../../services/api-config.json';
import { ToastrService } from 'ngx-toastr';

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

    constructor(private http: HttpClient, private _apiService: ApiService, private toaster: ToastrService) { }

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
            this.toaster.error('File size exceeds 1MB');
        }
        if (extension === 'xlsx' || extension === 'txt' || extension === 'xlsm') {
            if (file) {
                this.uploader = new Uploader(file);
            }
        } else {
            this.toaster.error('Invalid file type');
        }
    }

    // upload
    upload() {
        this.uploadOk = undefined;
        if (this.uploader.id == null) {
            return;
        }
        if (this.selectedPeriod === undefined) {
            this.toaster.error('Please select a period');
            return;
        }

        const selectedFile = this.uploader;
        if (selectedFile) {
            const formData = new FormData();
            formData.append(selectedFile.file.name, selectedFile.file);
            formData.append('PERIOD', this.selectedPeriod.toString());

            this._apiService.post(API.fileupload, formData)
                .subscribe(res => {
                    this.uploadOk = 'ok';
                    this.uploader = undefined;
                    if (res === true) {
                        this.toaster.success('Processing data...', 'File Uploaded Successfully');
                    }
                    else {
                        this.toaster.error('Template is not valid', 'Error!');
                    }
                },
                    err => {
                        this.toaster.error(`${err}`, 'File Uploaded Failed');
                        console.error(`Error occured retrieving resource canlendar ${err}`);
                    });
        }
    }

    // cancel all
    cancelAll() {
        this.uploader = undefined;
    }
}
