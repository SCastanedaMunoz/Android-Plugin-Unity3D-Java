package net.scastanedamunoz.unity;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.util.Log;

public class MyPlugin
{
    private static final MyPlugin ourInstance = new MyPlugin();

    private  static  final  String LOGTAG = "SCastanedaMunoz";

    public  static MyPlugin getInstance() { return ourInstance; }

    public  static Activity mainActivity;

    private long startTime;

    public interface AlertViewCallback
    {
        void onButtonTapped(int id);
    }

    private MyPlugin()
    {
        Log.i(LOGTAG, "Created my Plugin");
        startTime = System.currentTimeMillis();
    }

    public double getElapsedTime()
    {
        return  (System.currentTimeMillis() - startTime)/ 1000.0f;
    }

    public void showAlertView(String[] strings, final AlertViewCallback callback)
    {
        if(strings.length < 3)
        {
            Log.i(LOGTAG, "Error - expected at least 3 strings.");
            return;
        }

        DialogInterface.OnClickListener myClickListener = new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int id) {
                dialogInterface.dismiss();
                Log.i(LOGTAG, "Tapped: " + id);
                callback.onButtonTapped(id);
            }
        };

        AlertDialog alertDialog = new AlertDialog.Builder(mainActivity).setTitle(strings[0]).setMessage(strings[1]).setCancelable(false).create();

        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, strings[2], myClickListener);

        if(strings.length > 3)
            alertDialog.setButton(AlertDialog.BUTTON_NEGATIVE, strings[3], myClickListener);
        if(strings.length > 4)
            alertDialog.setButton(AlertDialog.BUTTON_POSITIVE, strings[4], myClickListener);

        alertDialog.show();
    }

}
