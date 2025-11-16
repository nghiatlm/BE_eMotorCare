

using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;

namespace eMototCare.BLL.Mappers
{
    public class RMAMapper : Profile
    {
        public RMAMapper() 
        {
            CreateMap<RMARequest, RMA>();
            CreateMap<RMAUpdateRequest, RMA>();
            CreateMap<RMA, RMAResponse>()
                .AfterMap((src, dest, ctx) =>
                {
                    var rmadetail = src.RMADetails?.FirstOrDefault();
                    var appointment = rmadetail?.EVCheckDetail?.EVCheck?.Appointment;

                    // Map Customer
                    if (appointment?.Customer != null)
                        dest.Customer = ctx.Mapper.Map<CustomerResponse>(appointment.Customer);

                    // Map Vehicle
                    var vehicle = appointment?.VehicleStage?.Vehicle ?? appointment?.Vehicle;
                    if (vehicle != null)
                        dest.Vehicle = ctx.Mapper.Map<VehicleResponse>(vehicle);
                });
        }
    }
}
