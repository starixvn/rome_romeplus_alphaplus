export interface ProfilePictureModel {
    ImageId: number;
    Image: string;
}

export interface GetPictureLibraryResponse {
    Images: ProfilePictureModel[];
    Pager: any;
}
