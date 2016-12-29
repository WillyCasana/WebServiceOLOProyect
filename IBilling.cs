using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceOLO
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IBilling" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IBilling
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        DetalleEnvio AddBilling(AuxBilling bill);

        
    }
}
