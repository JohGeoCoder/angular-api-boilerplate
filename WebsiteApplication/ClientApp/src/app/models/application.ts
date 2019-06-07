export class Application {
    id: number;
    name: string;
    shortDescription: string;
    longDescription: string;
    thumbnailFilePath: string;
    showOnStatus: boolean;

    constructor(application?: Application){
        if(application){
            this.id = application.id;
            this.name = application.name;
            this.shortDescription = application.shortDescription;
            this.longDescription = application.longDescription;
            this.showOnStatus = application.showOnStatus;
        }
        
    }
}
