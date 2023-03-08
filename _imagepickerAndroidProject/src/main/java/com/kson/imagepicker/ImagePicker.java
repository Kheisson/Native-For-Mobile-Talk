package com.kson.imagepicker;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.util.Log;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

public class ImagePicker extends Activity {
    public static final String TAG = "ImagePicker";
    public static final String SAVED_IMAGE_NAME = "selectedImage.jpg";
    private static final int PICK_IMAGE_REQUEST = 1;
    private static IUnityCallback unityCallback;

    //Called From Unity
    public static void setUnityCallback(IUnityCallback callback) {
            Log.d(TAG, "setUnityCallback: " + "ImagePicker on set unity callback");
            unityCallback = callback;
    }

    //Called From Unity
    public static void startImagePicker(Context context){
        Intent intent = new Intent(context, ImagePicker.class);
        context.startActivity(intent);
    }

    private void openGallery() {
        Log.d(TAG, "OpenGallery: " + "ImagePicker on open gallery");
        // Start an activity to pick an image from the gallery.
        Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
        intent.setType("image/*");
        startActivityForResult(intent, PICK_IMAGE_REQUEST);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Log.d(TAG, "onCreate: " + "ImagePicker on create");
        super.onCreate(savedInstanceState);
        openGallery();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        Log.d(TAG, "onActivityResult: " + "ImagePicker on activity result");
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == PICK_IMAGE_REQUEST && resultCode == RESULT_OK) {
            if (data != null) {
                Uri imageUri = data.getData();
                try {
                    String imagePath = saveImageToDownloadsFolder(imageUri);
                    unityCallback.onSuccess(imagePath);
                } catch (IOException e) {
                    unityCallback.onError(e.getMessage());
                    Log.d(TAG, "onActivityResult: " + "ImagePicker on activity result exception" + e.getMessage());
                }
            } else {
                    Log.e(TAG, "onActivityResult: " + "ImagePicker on activity result data null");
            }
        } else if (resultCode == RESULT_CANCELED) {
                Log.d(TAG, "onActivityResult: " + "ImagePicker on activity result canceled");
        }
        finish();
    }

    private String saveImageToDownloadsFolder(Uri imageUri) throws IOException {
        InputStream inputStream = getContentResolver().openInputStream(imageUri);
        File downloadDir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS);
        File selectedFile = new File(downloadDir, SAVED_IMAGE_NAME);
        try {
            OutputStream outputStream = new FileOutputStream(selectedFile);

            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.read(buffer)) != -1) {
                outputStream.write(buffer, 0, bytesRead);
            }

            outputStream.close();
            inputStream.close();
        } catch (Exception e) {
            Log.d(TAG, "saveImageToDownloadsFolder: " + "ImagePicker on save image to downloads folder exception" + e.getMessage());
        }

        String selectedFilePath = selectedFile.getAbsolutePath();
        Log.d(TAG, "onActivityResult: " + "ImagePicker on activity result temp file path: " + selectedFilePath);
        return selectedFilePath;
    }

}
