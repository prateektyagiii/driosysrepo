import { initOnLoad } from "apexcharts";

export class Sort
{
debugger;
private sortOrder=1;
private collator = new Intl.Collator(undefined,{
    numeric:true,
    sensitivity:"base",
});

constructor()
{

}

public startSort(property,order,type="")
{
    debugger;
    if(order==="desc")
    {
        this.sortOrder = -1;
    }
    return (a,b) => {
        if(type === "date")
        {
           return this.sortData(new Date(a[property]),new Date(b[property]));
        }
        else{
            return this.collator.compare(a[property],b[property]) * this.sortOrder;
        }
    }

}
private sortData(a,b)
{debugger;
    if(a<b)
    {
        return -1 * this.sortOrder;
    }
    else if(a>b)
    {
        return 1*this.sortOrder;
    }
    else
    {
        return 0 * this.sortOrder;
    }
  }
}